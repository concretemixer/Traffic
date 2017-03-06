#import "UnityAppController.h"
#import "Localytics.h"

@interface LocalyticsAppController : UnityAppController {}
@end

@implementation LocalyticsAppController

- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions
{
    [Localytics autoIntegrate:@"09014ed41fa1cf875213ba1-f889113a-f291-11e6-6245-008c9f3dc0ef" launchOptions:launchOptions];
 
    // If you are using Localytics Messaging include the following code to register for push notifications
    if ([application respondsToSelector:@selector(registerUserNotificationSettings:)])
    {
        UIUserNotificationType types = (UIUserNotificationTypeAlert | UIUserNotificationTypeBadge | UIUserNotificationTypeSound);
        UIUserNotificationSettings *settings = [UIUserNotificationSettings settingsForTypes:types categories:nil];
        [application registerUserNotificationSettings:settings];
        [application registerForRemoteNotifications];
    }
    else
    {
        [application registerForRemoteNotificationTypes:(UIRemoteNotificationTypeAlert | UIRemoteNotificationTypeBadge | UIRemoteNotificationTypeSound)];
    }

    [super application:application didFinishLaunchingWithOptions:launchOptions];

    return YES;
}

- (void)application:(UIApplication *)application handleWatchKitExtensionRequest:(NSDictionary *)userInfo reply:(void (^)(NSDictionary *))reply
{
    [Localytics handleWatchKitExtensionRequest:userInfo reply:reply];
}


@end

IMPL_APP_CONTROLLER_SUBCLASS( LocalyticsAppController )
