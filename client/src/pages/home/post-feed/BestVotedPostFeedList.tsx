import React, { FC } from "react";
import { useGetBestVotedPostsQuery } from "../../../api/postFeed/hooks/useGetBestVotedPostsQuery";
import { TimeRange } from "../../../api/models";
import { Box, VStack } from "@chakra-ui/react";
import ErrorMessage from "../../../components/ErrorMessage";
import { ApiError } from "../../../api/common/ApiError";
import FeedPost from "../feed-post/FeedPost";
import { Link } from "react-router-dom";
import PostSkeleton from "../../../components/PostSkeleton";

interface IProps {
   timeRange: TimeRange;
}

const BestVotedPostFeedList: FC<IProps> = ({ timeRange }) => {
   const { data: posts, isLoading, isError, error } = useGetBestVotedPostsQuery(timeRange);

   return (
      <Box>
         {isLoading && Array.from({ length: 4 }).map((_, index) => (
            <PostSkeleton key={index} />
         ))}
         <ErrorMessage show={isError} errorMessage={(error as ApiError)?.message ?? ""} />
         {!isLoading && posts && (
            <VStack spacing={4}>
               {
                  posts.map(post => (
                     <Link to={`/post/${post.id!}`}>
                        <FeedPost key={post.id} post={post} />
                     </Link>
                  ))
               }
            </VStack>
         )}
      </Box>
   );
};

export default BestVotedPostFeedList;