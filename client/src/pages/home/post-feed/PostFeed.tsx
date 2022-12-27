import React, { FC, useState } from "react";
import FeedTypeSelector from "../FeedTypeSelector";
import { TimeRange } from "../../../api/models";
import BestVotedPostFeedList from "./BestVotedPostFeedList";
import MostRecentPostFeedList from "./MostRecentPostFeedList";
import { Group } from "@mantine/core";

export enum FeedRankBy {
   Recent = "Recent",
   Best = "Best"
}

export interface PostFeedType {
   type: FeedRankBy,
   timeRange: TimeRange
}

const PostFeed: FC = () => {
   const [feedType, setFeedType] = useState<PostFeedType>({ timeRange: 3, type: FeedRankBy.Recent });

   return (
      <Group w={"550px"} spacing={24}>
         <FeedTypeSelector setFeedType={setFeedType} feedType={feedType} />
         {feedType.type === FeedRankBy.Best && <BestVotedPostFeedList timeRange={feedType.timeRange} />}
         {feedType.type === FeedRankBy.Recent && <MostRecentPostFeedList />}
      </Group>
   );
};

export default PostFeed;