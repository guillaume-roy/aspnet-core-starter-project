using System;

using ServerCore.Services;

namespace ServerTests.Fakes;

public class FakeUserSession : IUserSession
{
    public Guid UserId { get; set; }

    public FakeUserSession(Guid userId)
    {
        UserId = userId;
    }
}