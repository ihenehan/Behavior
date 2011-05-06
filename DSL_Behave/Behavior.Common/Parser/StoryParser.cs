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
        private List<Criterion> BeforeCriterion;
        private Criterion currentCriterion;

        public Story Story
        {
            get { return AssembleBlocks(); }
        }

        public StoryParser() { }

        public StoryParser(string file)
        {
            lines = File.ReadAllLines(file).ToList().PrepAllLines();

            blocks = BuildBlocks(lines);

            BeforeCriterion = new List<Criterion>();
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
            var story = new Story();

            foreach(Block block in blocks)
            {
                if (block.BlockType.Equals("Story"))
                    story = block.BuildStory();

                else if (block.BlockType.Equals("Before Story"))
                    story.BeforeStories.Add(block.BuildCriteria(ref BeforeCriterion));

                else if (block.BlockType.Equals("After Story"))
                    story.AfterStories.Add(block.BuildCriteria(ref BeforeCriterion));

                else if (block.BlockType.Equals("Criterion Common"))
                    story.CriterionCommon.Add(block.BuildCriteria(ref BeforeCriterion));

                else if (block.BlockType.Equals("Before Criterion"))
                    BeforeCriterion.Add(block.BuildCriteria(ref BeforeCriterion));

                else if (block.BlockType.Equals("Criterion"))
                {
                    currentCriterion = block.BuildCriteria(ref BeforeCriterion);
                    story.Criteria.Add(currentCriterion);
                }

                else if (block.BlockType.Equals("After Criterion"))
                    currentCriterion.AfterCriterion.Add(block.BuildCriteria(ref BeforeCriterion));

                else if (block.BlockType.Equals("Criterion Outline"))
                {
                    currentCriterion = block.BuildCriterionOutline(ref BeforeCriterion);
                    story.Criteria.Add(currentCriterion);
                }
            }
            return story;
        }
    }
}
