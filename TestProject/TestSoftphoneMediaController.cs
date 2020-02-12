using lindotnet.Classes;
using lindotnet.Classes.Component.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject
{
    [TestClass]
    public class TestSoftphoneMediaController
    {
        private static readonly TimeSpan ConnectionDelay = TimeSpan.FromSeconds(2);

        [TestMethod]
        public void TestCheckSoundDevicesAvability()
        {
            Softphone softphoneInstance = null;
            IEnumerable<Device> soundDevs = null;
            try
            {
                var testAccount = new Account(
                    login: "test",
                    password: "testpass",
                    host: "192.168.156.2",
                    accountName: "TestUser");

                softphoneInstance = new Softphone(testAccount);

                softphoneInstance.Connect();
                Task.Delay(ConnectionDelay).Wait();

                soundDevs = softphoneInstance.MediaController.GetSoundDevices();
            }
            finally
            {
                softphoneInstance.Disconnect();
            }
            Assert.IsTrue(soundDevs.Any());
        }

        [TestMethod]
        public void TestCheckVideoCaptureDevicesAvability()
        {
            Softphone softphoneInstance = null;
            IEnumerable<Device> videoDevs = null;
            try
            {
                var testAccount = new Account(
                    login: "test",
                    password: "testpass",
                    host: "192.168.156.2",
                    accountName: "TestUser");

                softphoneInstance = new Softphone(testAccount);

                softphoneInstance.Connect();
                Task.Delay(ConnectionDelay).Wait();

                videoDevs = softphoneInstance.MediaController.GetVideoCaptureDevices();
            }
            finally
            {
                softphoneInstance.Disconnect();
            }
            Assert.IsTrue(videoDevs.Any());
        }

        [TestMethod]
        public void TestSetVideoCaptureDevice()
        {
            Softphone softphoneInstance = null;
            IEnumerable<Device> videoDevs = null;
            Device mock = null;
            try
            {
                var testAccount = new Account(
                    login: "test",
                    password: "testpass",
                    host: "192.168.156.2",
                    accountName: "TestUser");

                softphoneInstance = new Softphone(testAccount);

                softphoneInstance.Connect();
                Task.Delay(ConnectionDelay).Wait();

                videoDevs = softphoneInstance.MediaController.GetVideoCaptureDevices();

                if (videoDevs.Any())
                {
                    mock = videoDevs.FirstOrDefault();
                    softphoneInstance.MediaController.VideoCaptureDevice = mock;
                }
            }
            finally
            {
                softphoneInstance.Disconnect();
            }
            Assert.AreSame(mock, softphoneInstance.MediaController.VideoCaptureDevice);
        }

        [TestMethod]
        public void TestSetAudioCaptureDevice()
        {
            Softphone softphoneInstance = null;
            IEnumerable<Device> audioDevs = null;
            Device mock = null;
            try
            {
                var testAccount = new Account(
                    login: "test",
                    password: "testpass",
                    host: "192.168.156.2",
                    accountName: "TestUser");

                softphoneInstance = new Softphone(testAccount);

                softphoneInstance.Connect();
                Task.Delay(ConnectionDelay).Wait();

                audioDevs = softphoneInstance.MediaController.GetSoundDevices();

                if (audioDevs.Any())
                {
                    mock = audioDevs.FirstOrDefault(dev => dev.Type == DeviceType.SoundCapture);
                    softphoneInstance.MediaController.AudioCaptureDevice = mock;
                }
            }
            finally
            {
                softphoneInstance.Disconnect();
            }
            Assert.AreSame(mock, softphoneInstance.MediaController.AudioCaptureDevice);
        }
    }
}