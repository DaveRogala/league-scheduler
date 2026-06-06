# Deferred Features

Features intentionally left for later to keep scope manageable. Revisit before each major release.

---

## User Profile

- **Configurable name fields**: Requirements say users should be able to add/remove name fields (e.g. drop middle name, add a fourth name). MVP uses fixed First / Middle / Last.
- **Additional Languages Spoken**: Multi-value field on user profile.
- **Preferred Language**: Defer until language list is configurable by admin.

---

## Player Profile

- **Tennis ratings**: NTRP (dropdown to .5), WTN (singles + doubles), UTR — all optional.
- **Hand preference**: Left, Right, Righty/Lefty (two forehands).
- **Preferred defence side**: Ad, Deuce.
- **Preferred court surface**: Hard, Grass, Clay.
- **Gender (for mixed doubles scheduling)**: Male, Female, Non-Binary, Self-described. Intentionally deferred pending design review on inclusive, respectful implementation.
- **Visibility controls**: Phone, email, address visibility toggles (default hidden from other players).
- **Address**: Optional, with city/postal code only option.

---

## Player / User Architecture

- **Link SeasonPlayer to AppUser**: `SeasonPlayer` entity needs a `UserId` FK to `AppUser` so scheduling preferences are tied to a real user account. Currently standalone.
- **Link LeaguePlayer to AppUser**: `LeaguePlayer` (league membership) should eventually reference `AppUser` directly rather than `SeasonPlayer`.
- **SeasonPlayer needs SeasonId FK**: Currently linked to leagues via `LeaguePlayer`; should be linked directly to a `Season` so per-season preferences can differ across seasons in the same league.

---

## SSO / Authentication

- **SSO providers**: Meta, Google, Microsoft, Okta — requirements specify SSO preference. Currently username/password only.
- **User impersonation**: System administrator impersonating users for troubleshooting.

---

## Notifications

- Email notifications
- SMS / text notifications
- Browser push notifications

---

## Scheduling

- **Pre-planned event de-confliction**: `PrePlannedEvent` is stored but not yet consumed by the scheduling algorithm.
- **Flexible court count**: Scale down to fewer courts when player count is low.
- **Mixed doubles gender scheduling**: Gender-aware matchmaking when league mode is mixed doubles.

---

## Calendar

- Connect to user calendars (Google, Outlook) for live schedule updates.
- Export to iCal format.

---

## Schedule Downloads

- CSV, Word, PDF export.

---

## Match Results

- As-scheduled result entry.
- Ad-hoc results (different groupings than scheduled).
- Photos on matches.

---

## League

- Player approval workflow (league requires approval before joining).
- Competitive mode: fixed teams, change approval, mandatory results.

---

## Mobile App

- Native Android and Apple apps.

---

## Admin

- System administrator global config panel.
- Language list configurable by admin.
- Configurable admin approval for league joins.

---

## Accessibility

- Full W3C / WCAG audit pass.

---

## Court

- Court photos/images.
