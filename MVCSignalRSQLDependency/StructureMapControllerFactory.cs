using System;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace MVCSignalRSQLDependency
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        private readonly IContainer _container;

        public StructureMapControllerFactory(IContainer container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                return null;

            return (IController)_container.GetInstance(controllerType);
        }
    }
}