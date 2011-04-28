using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;

namespace Behavior.Common.Repository
{
    public class TestRepository : IOldRepository
    {
        private string dataPath = "";
        private string projectsPath;
        private string storiesPath;
        private string scenariosPath;
        private string interactionsPath;
        private string dataItemsPath;
        private ISerializer itemSerializer;

        public string DataPath
        {
            get { return dataPath; }
            set
            {
                dataPath = value;

                if (!Directory.Exists(dataPath))
                    Directory.CreateDirectory(dataPath);

                projectsPath = dataPath + "\\project";
                
                if (!Directory.Exists(projectsPath))
                    Directory.CreateDirectory(projectsPath);

                storiesPath = dataPath + "\\story";

                if (!Directory.Exists(storiesPath))
                    Directory.CreateDirectory(storiesPath);

                scenariosPath = dataPath + "\\scenario";

                if (!Directory.Exists(scenariosPath))
                    Directory.CreateDirectory(scenariosPath);

                interactionsPath = dataPath + "\\interaction";

                if (!Directory.Exists(interactionsPath))
                    Directory.CreateDirectory(interactionsPath);

                dataItemsPath = dataPath + "\\dataitem";

                if (!Directory.Exists(dataItemsPath))
                    Directory.CreateDirectory(dataItemsPath);

                itemSerializer = new ItemJsonSerializer(dataPath);
            }
        }


        public Project CreateProject()
        {
            var project = new Project();

            var story = CreateStory(project);

            return project;
        }

        public Story CreateStory(Project parentProject)
        {
            var story = new Story() { ParentId = parentProject.Id };

            var scenario = CreateScenario(story);

            parentProject.StoryIds.Add(story.Id);

            parentProject.Stories.Add(story);

            SaveProject(parentProject);

            return story;
        }

        public Scenario CreateScenario(Story parentStory)
        {
            var scenario = new Scenario() { ParentId = parentStory.Id };

            var interaction = CreateInteraction(scenario);

            parentStory.ScenarioIds.Add(scenario.Id);

            parentStory.Scenarios.Add(scenario);

            SaveStory(parentStory);

            itemSerializer.Save(scenario, scenario.Id.ToString());

            return scenario;
        }

        public Interaction CreateInteraction(Scenario parentScenario)
        {
            int index = 0;

            if (parentScenario.Interactions.Count > 0)
            {
                var lastInteraction = parentScenario.Interactions.First(i => i.IsLast);

                lastInteraction.IsLast = false;

                itemSerializer.Save(lastInteraction, lastInteraction.Id.ToString());

                index = lastInteraction.Index + 1;
            }

            var interaction = new Interaction() { Index = index, IsLast = true, ParentId = parentScenario.Id };

            var dataItem = CreateDataItem(interaction);

            parentScenario.InteractionIds.Add(interaction.Id);

            parentScenario.Interactions.Add(interaction);

            SaveScenario(parentScenario);

            return interaction;
        }

        public DataItem CreateDataItem(Interaction parentInteraction)
        {
            int index = 0;

            if (parentInteraction.DataItems.Count > 0)
            {
                var lastDataItem = parentInteraction.DataItems.First(d => d.IsLast);

                lastDataItem.IsLast = false;

                itemSerializer.Save(lastDataItem, lastDataItem.Id.ToString());

                index = lastDataItem.Index + 1;
            }

            var dataItem = new DataItem() { Index = index, IsLast = true, ParentId = parentInteraction.Id };

            parentInteraction.DataItemIds.Add(dataItem.Id);

            parentInteraction.DataItems.Add(dataItem);

            parentInteraction.DataItems = parentInteraction.DataItems.OrderBy(d => d.Index).ToList();

            SaveInteraction(parentInteraction);

            return dataItem;
        }


        public void DeleteProject(Project project)
        {
            project.Stories.ForEach(s => DeleteStory(project, s));

            DeleteFile(project.Id, project.Type);
        }

        public Project DeleteStory(Project project, Story story)
        {
            project.StoryIds.Remove(story.Id);

            project.Stories.Remove(story);

            story.Scenarios.ForEach(s => DeleteScenario(story, s));

            DeleteFile(story.Id, story.Type);

            return project;
        }

        public Story DeleteScenario(Story story, Scenario scenario)
        {
            story.ScenarioIds.Remove(scenario.Id);

            story.Scenarios.Remove(scenario);

            scenario.Interactions.ForEach(i => DeleteInteraction(scenario, i));

            DeleteFile(scenario.Id, scenario.Type);

            return story;
        }

        public Scenario DeleteInteraction(Scenario scenario, Interaction interaction)
        {
            scenario.InteractionIds.Remove(interaction.Id);

            scenario.Interactions.Remove(interaction);

            scenario.Interactions = ReIndexInteractions(scenario.Interactions);

            interaction.DataItems.ForEach(d => DeleteDataItem(interaction, d));

            DeleteFile(interaction.Id, interaction.Type);

            return scenario;
        }

        public Interaction DeleteDataItem(Interaction interaction, DataItem dataItem)
        {
            interaction.DataItemIds.Remove(dataItem.Id);

            interaction.DataItems.Remove(dataItem);

            interaction.DataItems = ReIndexDataItems(interaction.DataItems);

            DeleteFile(dataItem.Id, dataItem.Type);

            return interaction;
        }

        public List<Interaction> ReIndexInteractions(List<Interaction> interactions)
        {
            if (interactions.Count > 0)
            {
                interactions = interactions.OrderBy(i => i.Index).ToList();

                interactions.ForEach(i => i.IsLast = false);

                interactions.Last().IsLast = true;
            }

            return interactions;
        }

        public List<DataItem> ReIndexDataItems(List<DataItem> dataItems)
        {
            if (dataItems.Count > 0)
            {
                dataItems = dataItems.OrderBy(d => d.Index).ToList();

                dataItems.ForEach(d => d.IsLast = false);

                dataItems.Last().IsLast = true;
            }

            return dataItems;
        }

        public void DeleteFile(Guid id, string type)
        {
            var path = dataPath + "\\" + type.ToLower() + "\\" + id.ToString() + ".jsn";

            if(File.Exists(path))
                File.Delete(path);
        }


        public void SaveProject(Project project)
        {
            itemSerializer.Save(project, project.Id.ToString());

            project.Stories.ForEach(s => itemSerializer.Save(s, s.Id.ToString()));
        }

        public void SaveStory(Story story)
        {
            itemSerializer.Save(story, story.Id.ToString());

            story.Scenarios.ForEach(s => itemSerializer.Save(s, s.Id.ToString()));
        }

        public void SaveScenario(Scenario scenario)
        {
            itemSerializer.Save(scenario, scenario.Id.ToString());

            scenario.Interactions.ForEach(i => itemSerializer.Save(i, i.Id.ToString()));
        }

        public void SaveInteraction(Interaction interaction)
        {
            itemSerializer.Save(interaction, interaction.Id.ToString());

            interaction.DataItems.ForEach(d => itemSerializer.Save(d, d.Id.ToString()));
        }

        public void SaveDataItem(DataItem dataItem)
        {
            itemSerializer.Save(dataItem, dataItem.Id.ToString());
        }


        public Project GetProject(Guid id)
        {
            var project = itemSerializer.Get<Project>(id);

            project.Stories = GetStories(project.StoryIds);

            return project;
        }

        public Story GetStory(Guid id)
        {
            var story = itemSerializer.Get<Story>(id);

            story.Scenarios = GetScenarios(story.ScenarioIds);

            return story;
        }

        public Scenario GetScenario(Guid id)
        {
            var scenario = itemSerializer.Get<Scenario>(id);

            scenario.Interactions = GetInteractions(scenario.InteractionIds);

            return scenario;
        }

        public Interaction GetInteraction(Guid id)
        {
            var interaction = itemSerializer.Get<Interaction>(id);

            interaction.DataItems = GetDataItems(interaction.DataItemIds);

            return interaction;
        }

        public DataItem GetDataItem(Guid id)
        {
            return itemSerializer.Get<DataItem>(id);
        }


        public List<Story> GetStories(List<Guid> ids)
        {
            var stories = itemSerializer.GetAll<Story>().Where<Story>(s => ids.Contains(s.Id)).ToList<Story>();

            stories.ForEach(s => s.Scenarios = GetScenarios(s.ScenarioIds));

            return stories;
        }

        public List<Scenario> GetScenarios(List<Guid> ids)
        {
            var scenarios = itemSerializer.GetAll<Scenario>().Where<Scenario>(s => ids.Contains(s.Id)).ToList<Scenario>();

            scenarios.ForEach(s => s.Interactions = GetInteractions(s.InteractionIds));

            return scenarios;
        }

        public List<Interaction> GetInteractions(List<Guid> ids)
        {
            var interactions = itemSerializer.GetAll<Interaction>().Where<Interaction>(i => ids.Contains(i.Id)).ToList<Interaction>();

            interactions.ForEach(i => i.DataItems = GetDataItems(i.DataItemIds));

            return interactions;
        }

        public List<DataItem> GetDataItems(List<Guid> ids)
        {
            var dataItems = itemSerializer.GetAll<DataItem>().Where<DataItem>(d => ids.Contains(d.Id)).ToList<DataItem>();

            dataItems = dataItems.OrderBy(d => d.Index).ToList();

            return dataItems;
        }


        public List<Project> GetProjects()
        {
            return itemSerializer.GetAll<Project>();
        }

        public List<Story> GetStories()
        {
            return itemSerializer.GetAll<Story>();
        }

        public List<Scenario> GetScenarios()
        {
            return itemSerializer.GetAll<Scenario>();
        }

        public List<Interaction> GetInteractions()
        {
            return itemSerializer.GetAll<Interaction>();
        }

        public List<DataItem> GetDataItems()
        {
            return itemSerializer.GetAll<DataItem>();
        }


        public List<Scenario> GetScenariosByTags(List<Scenario> scenarios, List<string> includeTags, List<string> excludeTags)
        {
            if (includeTags != null && includeTags.Count > 0)
            {
                var includeScenarios = scenarios.Where(s => includeTags.Any(i => s.Tags.Any(t => t.ToLower().Equals(i.ToLower())))).ToList();

                if (excludeTags != null && excludeTags.Count > 0)
                    return includeScenarios.Where(s => !excludeTags.Any(e => s.Tags.Any(t => t.ToLower().Equals(e.ToLower())))).ToList();

                return includeScenarios;
            }

            return new List<Scenario>();
        }

        public List<Story> GetStoriesWithTaggedScenarios(List<Story> stories, List<string> includeTags, List<string> excludeTags)
        {
            var taggedStories = new List<Story>();

            foreach (Story s in stories)
            {
                var taggedScenarios = GetScenariosByTags(GetScenarios(s.ScenarioIds), includeTags, excludeTags);

                if (taggedScenarios.Count > 0)
                {
                    taggedStories.Add(s);
                    s.Scenarios = taggedScenarios;
                }
            }

            return taggedStories;
        }
    }
}
