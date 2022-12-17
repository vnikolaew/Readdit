import React, { FC, useState } from "react";
import { Input, InputGroup, InputLeftAddon, InputRightAddon, Tag, TagCloseButton, TagLabel } from "@chakra-ui/react";
import { useFormikContext } from "formik";
import { PostApiPostsBody } from "../../api/models";
import { AddIcon } from "@chakra-ui/icons";

const TagsInput: FC = () => {
   const { setFieldValue, values: { Tags } } = useFormikContext<PostApiPostsBody>();
   const [tag, setTag] = useState<string>("");

   const isTagEmpty = tag === "";

   const handleKeyDown: React.KeyboardEventHandler<HTMLInputElement> = (e) => {
      if (e.key !== "Enter") return;
      e.preventDefault();

      const value = (e.target as any).value as string;
      if (!value.trim()) return;

      setFieldValue("Tags", [...Tags!, tag]);

      // @ts-ignore
      setTag("");
   };

   const removeTag = (index: number) => {
      setFieldValue("Tags", Tags!.filter((_, i) => i !== index));
   };

   const addTag = () => {
      if (tag === "") return;
      setFieldValue("Tags", [...Tags!, tag]);
      setTag("");
   };

   return (
      <InputGroup>
         <InputLeftAddon bgColor={"transparent"} px={1}>
            {Tags!.map((tag, index) => (
               <Tag
                  size={"sm"}
                  key={index}
                  mr={1}
                  py={1}
                  borderRadius="full"
                  variant="solid"
                  colorScheme="facebook"
               >
                  <TagLabel fontSize={14} mr={.5}>{tag}</TagLabel>
                  <TagCloseButton onClick={() => removeTag(index)} />
               </Tag>
            ))}
         </InputLeftAddon>
         <Input borderRight={"none"} borderLeft={"none"} onKeyDown={handleKeyDown} value={tag}
                onChange={(e) => setTag(e.target.value)}
                px={4}
                placeholder="Enter tags"
                type={"text"} />
         <InputRightAddon
            aria-disabled={tag === ""}
            _hover={isTagEmpty ? { opacity: .7 } : {}}
            onClick={() => addTag()}
            cursor={isTagEmpty ? "not-allowed" : "pointer"}
            bgColor={"transparent"}>
            <AddIcon />
         </InputRightAddon>
      </InputGroup>
   );
};

export default TagsInput;