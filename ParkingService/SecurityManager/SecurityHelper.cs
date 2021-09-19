using AuditingManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SecurityManager
{
    public class SecurityHelper
    {
        public static bool CheckAuthorization(string role, string userName, string action)
        {

            bool condition = false;

            if (Thread.CurrentPrincipal.IsInRole(role))
            {

                try
                {
                    Audit.AuthorizationSuccess(userName, action);

                    condition = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(userName, action, $"{action} method need {role} permission.");

                    condition = false;


                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return condition;
        }

        public static void AuditZoneOperationSucces(string zoneName, string operation)
        {

            try
            {
                Audit.ParkingZoneSuccess(zoneName, operation);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public static void AuditZoneOperationFailure(string operation, string error)
        {

            try
            {
                Audit.ParkingZoneFailure(operation, error);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public static void AuditAddParkingPenaltyTicketSuccess()
        {
            try
            {
                Audit.AddParkingPenaltyTicketSuccess();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void AuditAddParkingPenaltyTicketFailure(string error)
        {
            try
            {
                Audit.AddParkingPenaltyTicketFailure(error);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void AuditCheckPaymentSuccess()
        {
            try
            {
                Audit.CheckPaymentSuccess();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void AuditCheckPaymentFailure(string error)
        {
            try
            {
                Audit.CheckPaymentFailure(error);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void AuditPayParkingSuccess()
        {
            try
            {
                Audit.PayParkingSuccess();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void AuditPayParkingFailure(string error)
        {
            try
            {
                Audit.PayParkingFailure(error);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void AuditRemoveParkingPenaltyTicketSuccess()
        {
            try
            {
                Audit.RemoveParkingPenaltyTicketSuccess();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void AuditRemoveParkingPenaltyTicketFailure(string error)
        {
            try
            {
                Audit.RemoveParkingPenaltyTicketFailure(error);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void AuditReplicatorSuccess(string data)
        {
            try
            {
                Audit.ReplicatorSuccess(data);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        public static void AuditReplicatorFailure(string error)
        {
            try
            {
                Audit.ReplicatorFailure(error);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void AuditAuthenticationFailure(string error)
        {
            try
            {
                Audit.AuthenticationFailure(error);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        public static void AuditAuthenticationSuccess(string username)
        {

            try
            {
                Audit.AuthenticationSuccess(username);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }



    }
}
