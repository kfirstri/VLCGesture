﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoHandGesture
{
    public class Camera
    {
        const int WIDTH = 640;
        const int HEIGHT = 480;

        private PXCMSession _session;
        private PXCMSenseManager _mngr;
        private PXCMSenseManager.Handler _handler;

        private PXCMHandModule _hand;
        private PXCMHandData _handData;

        public Camera()
        {
            // Create the manager
            this._session = PXCMSession.CreateInstance();
            this._mngr = this._session.CreateSenseManager();

            // streammmm
            PXCMVideoModule.DataDesc desc = new PXCMVideoModule.DataDesc();
            desc.deviceInfo.streams = PXCMCapture.StreamType.STREAM_TYPE_COLOR | PXCMCapture.StreamType.STREAM_TYPE_DEPTH;
            this._mngr.EnableStreams(desc);
            //this._mngr.EnableStream(PXCMCapture.StreamType.STREAM_TYPE_COLOR, Camera.WIDTH, Camera.HEIGHT, 30);
            //this._mngr.EnableStream(PXCMCapture.StreamType.STREAM_TYPE_DEPTH, Camera.WIDTH, Camera.HEIGHT, 30);


            // Hands
            this._mngr.EnableHand();
            this._hand = this._mngr.QueryHand();
            this._handData = this._hand.CreateOutput();
            PXCMHandConfiguration conf = this._hand.CreateActiveConfiguration();
            conf.EnableGesture("spreadfingers", false);
            conf.EnableGesture("thumb-up", false);
            conf.SubscribeGesture(this.onFiredGesture);
            conf.SubscribeAlert(this.onFiredAlert);
            conf.ApplyChanges();

            // Set events
            this._handler = new PXCMSenseManager.Handler();
            this._handler.onModuleProcessedFrame = this.onModuleProcessedFrame;
            
            this._mngr.Init(this._handler);
        }

        public void Start()
        {
            this._mngr.StreamFrames(false);
        }

        private pxcmStatus onModuleProcessedFrame(int mid, PXCMBase module, PXCMCapture.Sample sample)
        {
            if (mid == PXCMHandModule.CUID)
            {
                this._handData.Update();
            }

            return pxcmStatus.PXCM_STATUS_NO_ERROR;
        }

        private void onFiredGesture(PXCMHandData.GestureData gestureData)
        {
            throw new NotImplementedException();
        }

        private void onFiredAlert(PXCMHandData.AlertData alertData)
        {
            throw new NotImplementedException();
        }

        ~Camera()
        {
            this._mngr.Dispose();
            this._session.Dispose();
        }
    }
}
