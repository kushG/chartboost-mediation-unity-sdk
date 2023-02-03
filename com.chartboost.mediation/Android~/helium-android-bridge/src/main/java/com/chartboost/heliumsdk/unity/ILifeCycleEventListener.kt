package com.chartboost.heliumsdk.unity

/**
 * Interface for sending real-time lifecycle events
 */
interface ILifeCycleEventListener {
    fun DidStart(error: String)
    fun DidReceiveILRD(impressionDataJson: String)
    fun DidReceivePartnerInitializationData(partnerInitializationData: String)
}
