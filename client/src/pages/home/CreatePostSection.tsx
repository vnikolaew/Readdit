import React, { FC } from "react";
import { Avatar, AvatarBadge, Box, Button, HStack, Input } from "@chakra-ui/react";
import { useGetUserDetailsQuery } from "../../api/users/hooks/useGetUserDetailsQuery";
import { useCurrentUser } from "../../api/common/hooks/useCurrentUser";
import { ArrowRightIcon } from "@chakra-ui/icons";
import { useNavigate } from "react-router-dom";
import { sleep } from "../../utils/sleep";

const CreatePostSection: FC = () => {
   const user = useCurrentUser();
   const navigate = useNavigate();
   const { data: userInfo, isLoading } = useGetUserDetailsQuery(user?.userId);

   return (
      <Box
         borderWidth={1}
         borderRadius={6}
         borderColor="gray"
         onClick={async () => {
            await sleep(200);
            navigate("/create/post");
         }}
         width={"450px"}
         _active={{ bgColor: "blackAlpha.900" }}
         py={6}
         px={2}
         p={2}
         color={"white"}
         bgColor={"blackAlpha.900"}
      >
         <HStack spacing={3}>
            <Avatar
               borderRadius={"50%"}
               size={"sm"}
               objectFit={"cover"}
               src={userInfo!.profile!.pictureUrl!}
            >
               <AvatarBadge
                  borderColor={"black"}
                  borderWidth={0.4}
                  boxSize={".7rem"}
                  bg={"green.500"}
               />
            </Avatar>
            <Input
               _hover={{
                  bgColor: "blackAlpha.200",
               }}
               borderWidth={1}
               borderColor={"gray"}
               bgColor={"blackAlpha.200"}
               size={"md"}
               variant={"filled"}
               placeholder={"Create Post"}
            />
            <Button
               onClick={async () => {
                  await sleep(200);
                  navigate("/create/post");
               }}
               ml={20}
               borderColor={"transparent"}
               borderWidth={1}
               _hover={{
                  bgColor: "transparent",
                  borderWidth: 1,
                  borderColor: "gray",
               }}
               bgColor={"transparent"}
            >
               <ArrowRightIcon />
            </Button>
         </HStack>
      </Box>
   );
};

export default CreatePostSection;
