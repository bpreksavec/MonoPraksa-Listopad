namespace WorkoutTracker.Service.DTOs;

public static class ValidationPatterns
{
    // Any Unicode letter, so names like "Čačić" or "Đurđević" are accepted; no digits, spaces or symbols
    public const string LettersOnly = @"^\p{L}+$";
    public const string LettersOnlyMessage = "{0} may only contain letters.";

    // name@domain.tld: no spaces, one @, a dot in the domain, and a top-level domain of at least 2 letters.
    // [EmailAddress] alone only checks for an @ with text on both sides, so "ben@testcom" would pass.
    public const string Email = @"^[^@\s]+@[^@\s.]+(\.[^@\s.]+)*\.[a-zA-Z]{2,}$";
    public const string EmailMessage = "Email must be a valid address, like name@example.com.";
}
