namespace Weapsy.Apps.Text.Domain
{
    public enum TextVersionStatus
    {
        Draft = 1,
        Published = 2,
        PreviouslyPublished = 3,
        DelayedPublish = 4,
        Rejected = 5,
        Deleted = 6
    }
}

//NotCreated Page or language has not been created
//Rejected The version was rejected rather than published, and returned to the writer.
//CheckedOut  The version is checked out and locked by a writer.
//CheckedIn A writer has checked in the version and waits for the version to be published.
//Published The currently published version.
//PreviouslyPublished This version has been published but is now replaced by a more recent version.
//DelayedPublish This version will be automatically published when the current time has passed the Start Publish date.