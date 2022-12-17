import React, { FC } from "react";
import { Box, BoxProps, SkeletonCircle, SkeletonText } from "@chakra-ui/react";

const PostSkeleton: FC<BoxProps> = (props) => {
   return (
      <Box mb={3} padding="6" boxShadow="lg" bg="white" {...props}>
         <SkeletonCircle size="10" />
         <SkeletonText
            width={"500px"}
            mt="4" noOfLines={4} spacing="4" skeletonHeight="2" />
      </Box>
   );
};

export default PostSkeleton;