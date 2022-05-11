dotnet clean
dotnet build
wt -M -d . dotnet run --project Autobarn.PricingServer; ^
split-pane -H -d . dotnet run --project Autobarn.PricingClient; ^
move-focus up; ^
split-pane -V -d . dotnet run --project Autobarn.Website; ^
move-focus down; ^
split-pane -H -d . dotnet run --project Autobarn.AuditLog; ^
move-focus down; ^
split-pane -H -d . dotnet run --project Autobarn.Notifier