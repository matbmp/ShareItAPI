DELETE FROM "PostUser" WHERE "UserFeedId" = {user.Id};
INSERT INTO "PostUser"("FeedPostsId", "UserFeedId")  (
	        SELECT "Posts"."Id", {user.Id} FROM "Sections"
	        JOIN "Users" ON "Sections"."UserId" = "Users"."Id"
	        JOIN "Posts" ON "Sections"."UserId" = "Users"."Id"
	        WHERE "UserId" = (SELECT "UserId" FROM "Users" WHERE "Id" = {user.Id})
	        ORDER BY "Posts"."CreatedDate" DESC
	        LIMIT 400
            );