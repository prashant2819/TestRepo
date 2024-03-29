﻿using Orchard.Mvc.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Orchard.ProjectManagement
{
    public class Routes : IRouteProvider
    {
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[]
            {
                new RouteDescriptor {
                                      Route = new Route(
                                                         "Admin/Contents/Create/ProjectTaskMgmt",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Orchard.ProjectManagement"},
                                                                                      {"controller", "ProjectAdmin"},
                                                                                      {"action", "List"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "Orchard.ProjectManagement"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                                      
                                                       

                };
        }


        
    }
}