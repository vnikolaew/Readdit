import React, { FC } from "react";
import { useGetMostRecentPostsQuery } from "../../../api/postFeed/hooks/useGetMostRecentPostsQuery";
import { Box, VStack } from "@chakra-ui/react";
import FeedPost from "../feed-post/FeedPost";
import ErrorMessage from "../../../components/ErrorMessage";
import { ApiError } from "../../../api/common/ApiError";
import { Link } from "react-router-dom";
import PostSkeleton from "../../../components/PostSkeleton";

const MostRecentPostFeedList: FC = () => {
   const { data: posts, isLoading, isError, error } = useGetMostRecentPostsQuery();

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
                     <Link key={post.id} to={`/post/${post.id!}`}>
                        <FeedPost post={post} />
                     </Link>
                  ))
               }
            </VStack>
         )}
      </Box>
   );
};

export default MostRecentPostFeedList;