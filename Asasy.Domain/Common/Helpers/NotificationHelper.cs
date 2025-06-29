using AAITHelper.ModelHelper;
using FirebaseAdmin.Messaging;

namespace Asasy.Domain.Common.Helpers
{
    public static class NotificationHelper
    {
        public static async Task<string> SendPushNewNotificationAsync(List<DeviceIdModel> device_Ids, Dictionary<string, string> data, string msg = "")
        {
            try
            {
                var result = "";
                foreach (DeviceIdModel device_Id in device_Ids)
                {
                    if (device_Id == null)
                    {
                        continue;
                    }

                    string deviceId = device_Id.DeviceId;

                    var message = new Message()
                    {
                        Notification = new Notification
                        {
                            Title = device_Id.ProjectName,
                            Body = msg
                        },

                        Token = deviceId,
                        Data = data


                    };
                    try
                    {
                        var messaging = FirebaseMessaging.DefaultInstance;
                        string response = await messaging.SendAsync(message);
                        result = response;
                    }
                    catch
                    {
                        continue;
                    }




                }

                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
