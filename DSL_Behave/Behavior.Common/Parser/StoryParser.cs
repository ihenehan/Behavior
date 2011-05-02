using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;
using Behavior.Common.Extensions;
using Behavior.Common.LookUps;

namespace Behavior.Common.Parser
{
    public class StoryParser
    {
        private List<string> lines;
        private Story story;
        private List<Tag> currentTags;
        private List<Scenario> BeforeScenarios;
        private Scenario currentScenario;
        private int currentLine;

        public Story Story
        {
            get { return ScanStory(); }
        }

        public StoryParser() { }

        public StoryParser(string file)
        {
            lines = File.ReadAllLines(file).ToList().PrepAllLines();

            story = new Story();

            currentTags = new List<Tag>();

            BeforeScenarios = new List<Scenario>();
        }

        public Story ScanStory()
        {
            currentLine = 0;

            while (lines[currentLine] != "<EOF>")
            {
                if (lines[currentLine].StartsWith("@"))
                {
                    currentTags.Add(new Tag(lines[currentLine].Replace("@", "")));

                    currentLine++;
                }

                else if (lines[currentLine].StartsWith("Story:"))
                    story = BuildStory(ref currentLine, ref currentTags);

                else if (lines[currentLine].StartsWith("Before Story:"))
                    story.BeforeStories.Add(BuildScenario(lines, ref currentLine, ref currentTags, "Before Story"));

                else if (lines[currentLine].StartsWith("After Story:"))
                    story.AfterStories.Add(BuildScenario(lines, ref currentLine, ref currentTags, "After Story"));

                else if (lines[currentLine].StartsWith("Scenario Common:"))
                    story.ScenarioCommon.Add(BuildScenario(lines, ref currentLine, ref currentTags, "Scenario Common"));

                else if (lines[currentLine].StartsWith("Before Scenario:"))
                    BeforeScenarios.Add(BuildScenario(lines, ref currentLine, ref currentTags, "Before Scenario"));

                else if (lines[currentLine].StartsWith("Scenario:"))
                {
                    currentScenario = BuildScenario(lines, ref currentLine, ref currentTags, "Scenario");
                    story.Scenarios.Add(currentScenario);
                }

                else if (lines[currentLine].StartsWith("After Scenario:"))
                    currentScenario.AfterScenarios.Add(BuildScenario(lines, ref currentLine, ref currentTags, "After Scenario"));

                else if (lines[currentLine].StartsWith("Scenario Outline:"))
                {
                    currentScenario = BuildScenarioOutline(lines, ref currentLine, ref currentTags);
                    story.Scenarios.Add(currentScenario);
                }

                else
                {
                    if (currentLine == lines.Count - 1)
                    {
                        currentLine = lines.Count - 1;
                        lines[currentLine] = "<EOF>";
                    }
                    else
                        currentLine++;
                }
            }
            
            return story;
        }

        public Story BuildStory(ref int currentLine, ref List<Tag> currentTags)
        {
            story.Name = lines[currentLine].Split(':')[1].Trim();

            currentLine++;

            var line = lines[currentLine];

             var matches = ParserStrings.BlockTypes.Any(t => line.StartsWith(t));

            while (!matches)
            {
                story.DescriptionLines.Add(lines[currentLine] + " ");

                currentLine++;

                line = lines[currentLine];

                matches = ParserStrings.BlockTypes.Any(t => line.StartsWith(t));
            }

            story.Tags.AddRange(currentTags);

            currentTags.Clear();

            return story;
        }

        public Scenario BuildScenario(List<string> lines, ref int currentLine, ref List<Tag> currentTags, string scenarioType)
        {
            var scenario = new Scenario() { ScenarioType = scenarioType };

            scenario.Tags.AddRange(currentTags);

            scenario.BeforeScenarios.AddRange(BeforeScenarios);

            BeforeScenarios.Clear();

            scenario.Name = lines[currentLine].Split(':')[1].Trim();

            currentLine++;

            var firstWord = lines[currentLine].Split(' ')[0].Trim();

            var matches = ParserStrings.Keywords.Any(t => firstWord.StartsWith(t));

            while (MatchesKeyword(lines[currentLine]) && currentLine < lines.Count)
            {
                var step = new ScenarioStep(lines[currentLine].Replace(firstWord, "").Trim());

                if (currentLine < lines.Count - 1)
                {
                    currentLine++;

                    firstWord = lines[currentLine].Split(' ')[0].Trim();

                    if (lines[currentLine].StartsWith("|"))
                    {
                        step.Table = new Table().Parse(lines, ref currentLine);

                        firstWord = lines[currentLine].Split(' ')[0].Trim();
                    }
                }
                else
                {
                    scenario.Steps.Add(step);

                    break;
                }

                scenario.Steps.Add(step);
            }

            currentTags.Clear();

            return scenario;
        }

        public bool MatchesKeyword(string toMatch)
        {
            var firstWord = lines[currentLine].Split(' ')[0].Trim();

            return ParserStrings.Keywords.Any(t => firstWord.StartsWith(t));
        }

        public Scenario BuildScenarioOutline(List<string> lines, ref int currentLine, ref List<Tag> currentTags)
        {
            var outline = new Scenario() { ScenarioType = "Scenario Outline" };

            outline.Tags.AddRange(currentTags);

            outline.BeforeScenarios.AddRange(BeforeScenarios);

            BeforeScenarios.Clear();

            outline.Name = lines[currentLine].Split(':')[1].Trim();

            currentLine++;

            var firstWord = lines[currentLine].Split(' ')[0].Trim();

            while (ParserStrings.Keywords.Contains(firstWord))
            {
                outline.Steps.Add(new ScenarioStep(lines[currentLine].Replace(firstWord, "").Trim()));

                currentLine++;

                firstWord = lines[currentLine].Split(' ')[0].Trim();
            }

            if (lines[currentLine].StartsWith("Test Data:"))
            {
                currentLine++;

                outline.Table = new Table().Parse(lines, ref currentLine);
            }

            currentTags.Clear();

            return outline;
        }
    }
}
