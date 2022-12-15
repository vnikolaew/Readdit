import React, { FC } from "react";
import { Divider, Flex, Heading } from "@chakra-ui/react";
import CreatePostForm from "./CreatePostForm";

const CreatePost: FC = () => {
   return (
      <Flex
         alignItems={"flex-start"}
         direction={"column"}
         width={"50%"}
         mt={6}
         mx={"auto"}
      >
         <Heading
            fontSize={24}
            fontWeight={"semibold"}
            color={"blackAlpha.800"}
         >
            Create a Post
         </Heading>
         <Divider
            my={2}
            orientation={"horizontal"}
            h={3}
            color={"blackAlpha.900"}
         />
         <CreatePostForm />
      </Flex>
   );
};

export default CreatePost;
