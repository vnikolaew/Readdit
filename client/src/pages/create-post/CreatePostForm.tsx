import React, { FC } from "react";
import CommunitySelectDropdown from "./CommunitySelectDropdown";
import { Form, Formik } from "formik";
import { PostApiPostsBody } from "../../api/models";
import { log } from "../../utils/logger";
import { validationSchema } from "./validationSchema";
import { Button, Flex, InputGroup, InputRightElement } from "@chakra-ui/react";
import FormField from "../register/FormField";
import FormTextarea from "../register/FormTextarea";
import ImageSelect from "../register/ImageSelect";
import TagsInput from "./TagsInput";

const CreatePostForm: FC = () => {
   return (
      <Formik<PostApiPostsBody>
         initialValues={{
            CommunityId: null!,
            Content: "",
            Media: null!,
            Tags: [],
            Title: "",
         }}
         validationSchema={validationSchema}
         onSubmit={async (values) => {
            log(values);
         }
         }>
         {({
              isSubmitting,
              handleSubmit,
              values: { Title, Content },
              errors: { Title: titleError, Content: contentError },
           }) => (
            <Form onSubmit={handleSubmit} style={{ width: "100%" }}>
               <Flex my={10} gap={6} direction={"column"} alignItems={"flex-start"}>
                  <CommunitySelectDropdown />
                  <InputGroup>
                     <FormField<PostApiPostsBody> placeholder={"Title"} name={"Title"} label={"Title"} />
                     <InputRightElement
                        fontWeight={titleError ? "semibold" : "medium"}
                        fontSize={12}
                        color={titleError ? "red" : "gray"}
                        mr={3}>
                        {Title.length} / 200
                     </InputRightElement>
                  </InputGroup>
                  <InputGroup mb={4}>
                     <FormTextarea<PostApiPostsBody>
                        placeholder={"Content (required)"}
                        name={"Content"}
                        label={"Content"} />
                     <InputRightElement
                        fontWeight={contentError ? "semibold" : "medium"}
                        fontSize={12}
                        color={contentError ? "red" : "gray"}
                        mr={3}>
                        {Content.length} / 200
                     </InputRightElement>
                  </InputGroup>
                  <TagsInput />
                  <ImageSelect name={"Media"} />
                  <Button fontSize={20} borderRadius={10}
                          cursor={"pointer"}
                          px={12}
                          py={6}
                          alignSelf={"flex-end"}
                          boxShadow={"lg"}
                          colorScheme={"facebook"} variant={"solid"}
                          type={"submit"}>Post</Button>
               </Flex>
            </Form>
         )}
      </Formik>
   );
};

export default CreatePostForm;