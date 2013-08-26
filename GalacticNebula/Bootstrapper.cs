using System.Web.Mvc;
using GalacticNebula.Models.Page;
using Microsoft.Practices.Unity;
using Unity.Mvc4;
using GalacticNebula.Models.Content;

namespace GalacticNebula
{
  public static class Bootstrapper
  {
    public static IUnityContainer Initialise()
    {
      var container = BuildUnityContainer();

      DependencyResolver.SetResolver(new UnityDependencyResolver(container));

      return container;
    }

    private static IUnityContainer BuildUnityContainer()
    {
      var container = new UnityContainer();

      // register all your components with the container here
      // it is NOT necessary to register your controllers

      // e.g. container.RegisterType<ITestService, TestService>();    
      container.RegisterType<DIPageInterface, DIPageRepository>();
      container.RegisterType<DIContentInterface, DIContentRepository>();
      container.RegisterType<DIContentMoreDetailInterface, DIContentMoreDetailRepository>();    
      RegisterTypes(container);

      return container;
    }

    public static void RegisterTypes(IUnityContainer container)
    {
    
    }
  }
}