using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;

namespace Behavior.Common.Repository
{
    public interface IOldRepository
    {
        string DataPath { get; set; }
        
        Project CreateProject();
        Story CreateStory(Project parentProject);
        Scenario CreateScenario(Story parentStory);
        Interaction CreateInteraction(Scenario parentScenario);
        DataItem CreateDataItem(Interaction parentInteraction);

        void DeleteProject(Project project);
        Project DeleteStory(Project project, Story story);
        Story DeleteScenario(Story story, Scenario scenario);
        Scenario DeleteInteraction(Scenario scenario, Interaction interaction);
        Interaction DeleteDataItem(Interaction interaction, DataItem dataItem);

        void SaveProject(Project project);
        void SaveStory(Story story);
        void SaveScenario(Scenario scenario);
        void SaveInteraction(Interaction interaction);
        void SaveDataItem(DataItem dataItem);

        Project GetProject(Guid id);
        Story GetStory(Guid id);
        Scenario GetScenario(Guid id);
        Interaction GetInteraction(Guid id);
        DataItem GetDataItem(Guid id);
        
        List<Project> GetProjects();
        List<Story> GetStories();
        List<Scenario> GetScenarios();
        List<Interaction> GetInteractions();
        List<DataItem> GetDataItems();
        
        List<Story> GetStories(List<Guid> ids);
        List<Scenario> GetScenarios(List<Guid> ids);
        List<Interaction> GetInteractions(List<Guid> ids);
        List<DataItem> GetDataItems(List<Guid> ids);

        List<Story> GetStoriesWithTaggedScenarios(List<Story> stories, List<string> includeTags, List<string> excludeTags);
        List<Scenario> GetScenariosByTags(List<Scenario> scenarios, List<string> includeTags, List<string> excludeTags);
    }
}
