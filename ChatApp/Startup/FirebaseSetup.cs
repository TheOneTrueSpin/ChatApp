using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace ChatApp.Startup;

public static class FirebaseSetup
{
    public static IServiceCollection AddFirebase(this IServiceCollection services)
    {
        string? firebaseServiceAccountKeyPath = Environment.GetEnvironmentVariable("FIREBASE_SERVICE_ACCOUNT_KEY_JSON");

        if (firebaseServiceAccountKeyPath is null)
        {
            throw new Exception("FIREBASE_SERVICE_ACCOUNT_KEY_JSON environment variable is null");
        }

        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromJson(firebaseServiceAccountKeyPath),
        });

        return services;
    }
}
