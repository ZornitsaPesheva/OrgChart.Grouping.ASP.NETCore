# OrgChart Grouping with ASP.NET Core

This demo demostrates gow to persist groups on the server

        function updateTags(sender, tags) {
            $.post("@Url.Action("UpdateTags")", { tags: JSON.stringify(tags) });
        }
