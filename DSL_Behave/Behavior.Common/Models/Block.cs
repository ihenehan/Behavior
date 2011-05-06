using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.LookUps;

namespace Behavior.Common.Models
{
    public class Block
    {
        public string BlockType { get; set; }
        public string Name { get; set; }
        public List<string> Lines { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public List<Tag> Tags { get; set; }

        public Block()
        {
            Lines = new List<string>();
            Tags = new List<Tag>();
        }

        public Story BuildStory()
        {
            var story = new Story();

            story.Name = Name;

            story.DescriptionLines = Lines;

            story.Tags = Tags;

            return story;
        }

        public Criterion BuildCriteria(ref List<Criterion> beforeCriteria)
        {
            var criterion = new Criterion(this);

            criterion.BeforeCriteria.AddRange(beforeCriteria);

            beforeCriteria.Clear();

            var currentLine = 0;

            if (Lines.Count > 0)
            {
                var matches = LanguageElements.Keywords.Any(t => FirstWord(currentLine).StartsWith(t));

                while (MatchesKeyword(Lines[currentLine]) && currentLine < Lines.Count)
                {
                    var step = new CriterionStep(FirstWord(currentLine), Lines[currentLine].Replace(FirstWord(currentLine), "").Trim());

                    if (currentLine < Lines.Count - 1)
                    {
                        currentLine++;

                        if (Lines[currentLine].StartsWith(LanguageElements.TableDelimiter))
                            step.Table = new Table().Parse(Lines, ref currentLine);
                    }
                    else
                    {
                        criterion.Steps.Add(step);
                        break;
                    }

                    criterion.Steps.Add(step);
                }
            }
            return criterion;
        }

        public Criterion BuildCriterionOutline(ref List<Criterion> beforeCriteria)
        {
            var outline = new Criterion(this);

            outline.BeforeCriteria.AddRange(beforeCriteria);

            beforeCriteria.Clear();

            var currentLine = 0;

            if (Lines.Count > 0)
            {
                while (LanguageElements.Keywords.Any(e => e.Equals(FirstWord(currentLine))))
                {
                    outline.Steps.Add(new CriterionStep(FirstWord(currentLine), Lines[currentLine].Replace(FirstWord(currentLine), "").Trim()));

                    currentLine++;
                }

                if (Lines[currentLine].StartsWith("Test Data:"))
                {
                    currentLine++;

                    outline.Table = new Table().Parse(Lines, ref currentLine);
                }
            }

            return outline;
        }

        public string FirstWord(int lineIndex)
        {
            return Lines[lineIndex].Split(' ')[0].Trim();
        }

        public bool MatchesKeyword(string line)
        {
            var firstWord = line.Split(' ')[0].Trim();

            return LanguageElements.Keywords.Any(t => firstWord.StartsWith(t));
        }
    }
}
