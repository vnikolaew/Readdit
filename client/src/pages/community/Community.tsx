import React, { FC } from "react";
import { useParams } from "react-router-dom";
import { useGetCommunityDetailsQuery } from "../../api/communities";
import { Box, Flex, Spinner } from "@chakra-ui/react";

const Community: FC = () => {
   const { communityName } = useParams();
   const { data: communityDetails, isLoading } =
      useGetCommunityDetailsQuery(communityName);

   return (
      <Flex>
         <Box>Welcome to {communityName}!</Box>
         {isLoading ? (
            <Box>
               <Spinner size={"md"} />
            </Box>
         ) : (
            <Box>{JSON.stringify(communityDetails, null, 2)}</Box>
         )}
      </Flex>
   );
};

export default Community;