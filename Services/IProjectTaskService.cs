using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ProjectManagement.Models;
using Orchard.Data;
using Orchard.ContentManagement;
using Orchard.Core.Title.Models;

namespace Orchard.ProjectManagement.Services
{

    public interface IProjectTaskService : IDependency
    {
        IEnumerable<TaskMgmtPartRecord> GetTasks();
        IEnumerable<ProjectPart> Get() ;
        IEnumerable<ProjectPart> Get(VersionOptions versionOptions);
        IEnumerable<TaskPart> GetTask(VersionOptions versionOptions);
        ContentItem Get(int id, VersionOptions versionOptions);
        ContentItem GetTask(int id, VersionOptions versionOptions);
        
    }

    public class ProjectTaskService : IProjectTaskService
    {
        private readonly IRepository<TaskMgmtPartRecord> _tasks;
        private readonly IContentManager _contentManager;
        public ProjectTaskService(IRepository<TaskMgmtPartRecord> taskRepository, IContentManager contentManager)
        {
            _tasks = taskRepository;
            _contentManager = contentManager;
        }
        public IEnumerable<TaskMgmtPartRecord> GetTasks()
        {
            return _tasks.Table.ToList();

        }

        public IEnumerable<ProjectPart> Get(VersionOptions versionOptions)
        {
            return _contentManager.Query<ProjectPart, TaskMgmtPartRecord>(versionOptions)
                .Join<TitlePartRecord>()
                .OrderBy(br => br.Title)
                .List();
        }
        public ContentItem Get(int id, VersionOptions versionOptions)
        {
            var projectPart = _contentManager.Get<ProjectPart>(id, versionOptions);
            return projectPart == null ? null : projectPart.ContentItem;
        }

       

        public IEnumerable<TaskPart> GetTask(VersionOptions versionOptions)
        {
            return _contentManager.Query<TaskPart, TaskMgmtPartRecord>(versionOptions)
                .Join<TitlePartRecord>()
                .OrderBy(br => br.Title)
                .List();
        }
        public IEnumerable<ProjectPart> Get()
        {
            return Get(VersionOptions.Published);
        }

        public ContentItem GetTask(int id, VersionOptions versionOptions)
        {
            var taskPart = _contentManager.Get<TaskPart>(id, versionOptions);
            return taskPart == null ? null : taskPart.ContentItem;
        }

        

    }



}