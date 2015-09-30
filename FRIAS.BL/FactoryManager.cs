using FRIAS.BL.Managers;

namespace FRIAS.BL
{
    public class FactoryManager
    {
        // types of managers
        public enum Type
        {
            Cable, Component, CableBlock, Disposition, Drawing, FireArea, FireZone, FireRoom, NPO, NSCA, Power, PRA, Route, User, Document
        }

        // create manager of a given type
        public static IFactoryManager CreateManager(Type id)
        {
            IFactoryManager mgr = null;
            switch (id)
            {
                case Type.User:
                    mgr = new UserManager();
                    break;
                case Type.Component:
                    mgr = new ComponentManager();
                    break;
                case Type.Cable:
                    mgr = new CableManager();
                    break;
                case Type.CableBlock:
                    mgr = new BlockDiagramManager();
                    break;
                case Type.Drawing:
                    mgr = new DrawingManager();
                    break;
                case Type.Power:
                    mgr = new PowerManager();
                    break;
                case Type.Route:
                    mgr = new RouteManager();
                    break;
                case Type.FireArea:
                    mgr = new FireAreaManager();
                    break;
                case Type.FireZone:
                    mgr = new FireZoneManager();
                    break;
                case Type.FireRoom:
                    mgr = new FireRoomManager();
                    break;
                case Type.Disposition:
                    mgr = new DispositionManager();
                    break;
                case Type.NSCA:
                    mgr = new NSCAManager();
                    break;
                case Type.PRA:
                    mgr = new PRAManager();
                    break;
                case Type.NPO:
                    mgr = new NPOManager();
                    break;
                case Type.Document:
                    mgr = new DocumentManager();
                    break;
                default:
                    break;
            }
            return mgr;
        }
    }
}
