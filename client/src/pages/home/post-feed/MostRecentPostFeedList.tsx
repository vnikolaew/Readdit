import React, { FC } from "react";
import { useGetMostRecentPostsQuery } from "../../../api/postFeed/hooks/useGetMostRecentPostsQuery";
import FeedPost from "../feed-post/FeedPost";
import ErrorMessage from "../../../components/ErrorMessage";
import { ApiError } from "../../../api/common/ApiError";
import Link from "../../../components/Link";
import PostSkeleton from "../../../components/PostSkeleton";
import { Group, Stack } from "@mantine/core";

const MostRecentPostFeedList: FC = () => {
   const { data: posts, isLoading, isError, error } = useGetMostRecentPostsQuery();

   return (
      <Group w={"100%"}>
         {isLoading && Array.from({ length: 4 }).map((_, index) => <PostSkeleton key={index} />)}
         <ErrorMessage show={isError} errorMessage={(error as ApiError)?.message ?? ""} />
         {!isLoading && posts && (
            <Stack spacing={18}>
               {posts.map((post) => (
                  <Link key={post.id} to={`/post/${post.id!}`}>
                     <FeedPost post={post} />
                  </Link>
               ))}
            </Stack>
         )}
      </Group>
   );
};

export default MostRecentPostFeedList;