import React, { FC, useState } from "react";
import { VStack } from "@chakra-ui/react";
import FeedTypeSelector from "../FeedTypeSelector";
import { TimeRange } from "../../../api/models";
import BestVotedPostFeedList from "./BestVotedPostFeedList";
import MostRecentPostFeedList from "./MostRecentPostFeedList";

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
      <VStack width={"550px"} spacing={4}>
         <FeedTypeSelector setFeedType={setFeedType} feedType={feedType} />
         {feedType.type === FeedRankBy.Best && <BestVotedPostFeedList timeRange={feedType.timeRange} />}
         {feedType.type === FeedRankBy.Recent && <MostRecentPostFeedList />}
      </VStack>
   );
};

export default PostFeed;