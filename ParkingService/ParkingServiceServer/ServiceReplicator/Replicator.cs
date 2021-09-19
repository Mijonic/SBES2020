using CryptographyManager;
using ParkingServiceServer.RepositoryServices;
using ServiceContracts;
using ServiceContracts.DataModels;
using System;
using System.Collections.Generic;

namespace ParkingServiceServer.ServiceReplicator
{
	public class Replicator : IReplicator
	{
		private CarRepositoryService carRepoService = new CarRepositoryService();
		private PaymentRepositoryService paymentRepoService = new PaymentRepositoryService();
		private ZoneRepositoryService zoneRepoService = new ZoneRepositoryService();


		public KeyValuePair<byte[], byte[]> TransferCars()
		{
            List<Car> cars = carRepoService.GetAll();
			byte[] encrypted = CryptographyService<Car>.Encrypt(cars);
            byte[] signed = CryptographyService<Car>.SignData(cars);

            return new KeyValuePair<byte[], byte[]>(signed, encrypted);
		}

		public KeyValuePair<byte[], byte[]> TransferPayments()
		{
            List<Payment> payments = paymentRepoService.GetAll();
            byte[] encrypted = CryptographyService<Payment>.Encrypt(payments);
            byte[] signed = CryptographyService<Payment>.SignData(payments);

            return new KeyValuePair<byte[], byte[]>(signed, encrypted);
        }

		public KeyValuePair<byte[], byte[]> TransferZones()
		{
            List<ParkingZone> zones = zoneRepoService.GetAll();
            byte[] encrypted = CryptographyService<ParkingZone>.Encrypt(zones);
            byte[] signed = CryptographyService<ParkingZone>.SignData(zones);

            return new KeyValuePair<byte[], byte[]>(signed, encrypted);
        }
	}
}
