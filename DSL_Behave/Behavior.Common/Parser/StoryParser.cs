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
        private List<Block> blocks;
        private Story story;
        private List<Scenario> BeforeScenarios;
        private Scenario currentScenario;

        public Story Story
        {
            get { return AssembleBlocks(); }
        }

        public StoryParser() { }

        public StoryParser(string file)
        {
            lines = File.ReadAllLines(file).ToList().PrepAllLines();

            blocks = BuildBlocks(lines);

            story = new Story();

            BeforeScenarios = new List<Scenario>();
        }

        public List<Block> BuildBlocks(List<string> lines)
        {
            blocks = new List<Block>();

            var currentLine = 0;

            var currentTags = new List<Tag>();

            var block = new Block();

            foreach (string line in lines)
            {
                if (LanguageElements.BlockTypes.Any(t => line.StartsWith(t)))
                {
                    block.EndIndex = currentLine - 1;

                    if (!string.IsNullOrEmpty(block.BlockType))
                        blocks.Add(FinishBlock(block, currentTags, lines));

                    block = new Block();

                    block.BlockType = line.Split(':')[0].Trim();

                    block.Name = line.Split(':')[1].Trim();

                    block.StartIndex = currentLine;

                    currentTags = GetTags(lines, currentLine);
                }

                currentLine++;
            }

            block.EndIndex = currentLine - 1;

            blocks.Add(FinishBlock(block, currentTags, lines));

            return blocks;
        }

        public Block FinishBlock(Block block, List<Tag> tags, List<string> lines)
        {
            block.Tags = tags;

            block.Lines.AddRange(lines.GetRange(block.StartIndex + 1, block.EndIndex - block.StartIndex));

            block.Lines.RemoveAll(l => l.StartsWith(LanguageElements.TagToken));

            return block;
        }

        public List<Tag> GetTags(List<string> lines, int currentLine)
        {
            var tags = new List<Tag>();

            for (int i = currentLine - 1; i >= 0; i--)
                if (lines[i].StartsWith(LanguageElements.TagToken))
                    tags.Add(new Tag(lines[i].Replace(LanguageElements.TagToken, "").Trim()));
                else
                    break;

            return tags;
        }

        public Story AssembleBlocks()
        {
            foreach(Block block in blocks)
            {
                if (block.BlockType.Equals("Story"))
                    story = block.BuildStory();

                else if (block.BlockType.Equals("Before Story"))
                    story.BeforeStories.Add(block.BuildScenario(ref BeforeScenarios));

                else if (block.BlockType.Equals("After Story"))
                    story.AfterStories.Add(block.BuildScenario(ref BeforeScenarios));

                else if (block.BlockType.Equals("Scenario Common"))
                    story.ScenarioCommon.Add(block.BuildScenario(ref BeforeScenarios));

                else if (block.BlockType.Equals("Before Scenario"))
                    BeforeScenarios.Add(block.BuildScenario(ref BeforeScenarios));

                else if (block.BlockType.Equals("Scenario"))
                {
                    currentScenario = block.BuildScenario(ref BeforeScenarios);
                    story.Scenarios.Add(currentScenario);
                }

                else if (block.BlockType.Equals("After Scenario"))
                    currentScenario.AfterScenarios.Add(block.BuildScenario(ref BeforeScenarios));

                else if (block.BlockType.Equals("Scenario Outline"))
                {
                    currentScenario = block.BuildScenarioOutline(BeforeScenarios);
                    story.Scenarios.Add(currentScenario);
                }
            }
            return story;
        }
    }
}
