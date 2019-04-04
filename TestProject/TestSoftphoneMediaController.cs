using lindotnet.Classes;
using lindotnet.Classes.Component.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
	[TestClass]
	public class TestSoftphoneMediaController
	{
		private static readonly TimeSpan ConnectionDelay = TimeSpan.FromSeconds(2);
		private static Softphone _softphoneInstance;

		[ClassInitialize]
		private void BeforeTestsStart()
		{
			var testAccount = new Account(
					login: "test",
					password: "testpass",
					host: "officesip.local",
					accountName: "TestUser");

			_softphoneInstance = new Softphone(testAccount);

			_softphoneInstance.Connect();

			Task.Delay(ConnectionDelay).Wait();
		}

		[TestMethod]
		public void TestCheckSoundDevicesAvability()
		{
			var soundDevs = _softphoneInstance.MediaController.GetSoundDevices();

			Assert.IsTrue(soundDevs.Any());
		}

		[TestMethod]
		public void TestCheckVideoCaptureDevicesAvability()
		{
			var videoDevs = _softphoneInstance.MediaController.GetVideoCaptureDevices();

			Assert.IsTrue(videoDevs.Any());
		}

		[TestMethod]
		public void TestSetVideoCaptureDevice()
		{
			var videoDevs = _softphoneInstance.MediaController.GetVideoCaptureDevices();
			var mock = videoDevs.FirstOrDefault();

			_softphoneInstance.MediaController.VideoCaptureDevice = mock;

			Assert.AreSame(mock, _softphoneInstance.MediaController.VideoCaptureDevice);
		}

		[TestMethod]
		public void TestSetAudioCaptureDevice()
		{
			var audioDevs = _softphoneInstance.MediaController.GetSoundDevices();
			var mock = audioDevs.FirstOrDefault(dev => dev.Type == DeviceType.SoundCapture);

			_softphoneInstance.MediaController.AudioCaptureDevice = mock;
			Assert.AreSame(mock, _softphoneInstance.MediaController.AudioCaptureDevice);
		}

		[ClassCleanup]
		private void AfterTests()
		{
			_softphoneInstance?.Disconnect();
		}
	}
}
