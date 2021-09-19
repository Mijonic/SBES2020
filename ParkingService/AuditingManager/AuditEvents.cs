using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace AuditingManager
{
    public enum AuditEventTypes
    {
        AuthenticationSuccess = 0,
        AuthorizationSuccess = 1,
        AuthorizationFailed = 2,
        ParkingZoneSuccess = 3,
        ParkingZoneFailure = 4,
        PayParkingSuccess = 5,
        PayParkingFailure = 6,
        CheckPaymentSuccess = 7,
        CheckPaymentFailure = 8,
        AddParkingPenaltyTicketSuccess = 9,
        AddParkingPenaltyTicketFailure = 10,
        RemoveParkingPenaltyTicketSuccess = 11,
        RemoveParkingPenaltyTicketFailure = 12,
        ReplicatorSuccess = 13,
        ReplicatorFailure = 14,
        AuthenticationFailure = 15
    }

    public class AuditEvents
    {
        private static ResourceManager resourceManager = null;
        private static object resourceLock = new object();

        private static ResourceManager ResourceMgr
        {
            get
            {
                lock (resourceLock)
                {
                    if (resourceManager == null)
                    {
                        resourceManager = new ResourceManager
                            (typeof(AuditEventFile).ToString(),
                            Assembly.GetExecutingAssembly());
                    }
                    return resourceManager;
                }
            }
        }

        public static string AuthenticationSuccess
        {
            get
            {   
                return ResourceMgr.GetString(AuditEventTypes.AuthenticationSuccess.ToString());
            }
        }

        public static string AuthenticationFailure
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AuthenticationFailure.ToString());
            }
        }

        public static string AuthorizationSuccess
        {
            get
            {            
                return ResourceMgr.GetString(AuditEventTypes.AuthorizationSuccess.ToString());
            }
        }

        public static string AuthorizationFailed
        {
            get
            {     
                return ResourceMgr.GetString(AuditEventTypes.AuthorizationFailed.ToString());
            }
        }

        public static string ParkingZoneSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.ParkingZoneSuccess.ToString());
            }
        }


        public static string ParkingZoneFailure
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.ParkingZoneFailure.ToString());
            }
        }

        public static string PayParkingSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.PayParkingSuccess.ToString());
            }
        }

        public static string PayParkingFailure
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.PayParkingFailure.ToString());
            }
        }

        public static string CheckPaymentSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.CheckPaymentSuccess.ToString());
            }
        }

        public static string CheckPaymentFailure
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.CheckPaymentFailure.ToString());
            }
        }


        public static string AddParkingPenaltyTicketSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AddParkingPenaltyTicketSuccess.ToString());
            }
        }

        public static string AddParkingPenaltyTicketFailure
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AddParkingPenaltyTicketFailure.ToString());
            }
        }

        public static string RemoveParkingPenaltyTicketSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.RemoveParkingPenaltyTicketSuccess.ToString());
            }
        }

        public static string RemoveParkingPenaltyTicketFailure
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.RemoveParkingPenaltyTicketFailure.ToString());
            }
        }

        public static string ReplicatorSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.ReplicatorSuccess.ToString());
            }
        }

        public static string ReplicatorFailure
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.ReplicatorFailure.ToString());
            }
        }
      
    }
}
