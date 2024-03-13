namespace Club58Bot;

public class MockClubEventService : IClubEventService
{
    public IEnumerable<ClubEvent> GetEvents()
    {
        return
        [
            new ClubEvent { Id = 1, Name = "Event 1" },
            new ClubEvent { Id = 2, Name = "Event 2" }
        ];
    }
}
