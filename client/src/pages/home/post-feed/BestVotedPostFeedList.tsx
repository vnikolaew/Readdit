import React, { FC } from "react";
import { useGetBestVotedPostsQuery } from "../../../api/postFeed/hooks/useGetBestVotedPostsQuery";
import { TimeRange } from "../../../api/models";
import ErrorMessage from "../../../components/ErrorMessage";
import { ApiError } from "../../../api/common/ApiError";
import FeedPost from "../feed-post/FeedPost";
import Link from "../../../components/Link";
import PostSkeleton from "../../../components/PostSkeleton";
import { Group } from "@mantine/core";

interface IProps {
   timeRange: TimeRange;
}

const BestVotedPostFeedList: FC<IProps> = ({ timeRange }) => {
   const { data: posts, isLoading, isError, error } = useGetBestVotedPostsQuery(timeRange);

   return (
      <Group spacing={6}>
         {isLoading && Array.from({ length: 4 }).map((_, index) => <PostSkeleton key={index} />)}
         <ErrorMessage show={isError} errorMessage={(error as ApiError)?.message ?? ""} />
         {!isLoading && posts && (
            <Group spacing={10}>
               {posts.map((post) => (
                  <Link key={post.id} to={`/post/${post.id!}`}>
                     <FeedPost post={post} />
                  </Link>
               ))}
            </Group>
         )}
      </Group>
   );
};

export default BestVotedPostFeedList;
