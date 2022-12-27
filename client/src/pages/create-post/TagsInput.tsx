import React, { FC, useState } from "react";
import { useFormikContext } from "formik";
import { PostApiPostsBody } from "../../api/models";
import { MultiSelect, Text } from "@mantine/core";
import { AddIcon } from "@chakra-ui/icons";

const TagsInput: FC = () => {
   const { setFieldValue, values: { Tags } } = useFormikContext<PostApiPostsBody>();
   const [tag, setTag] = useState<string>("");

   const handleKeyDown: React.KeyboardEventHandler<HTMLInputElement> = (e) => {
      if (e.key !== "Enter") return;
      e.preventDefault();

      const value = (e.target as any).value as string;
      if (!value.trim()) return;

      setFieldValue("Tags", [...Tags!, tag]);

      // @ts-ignore
      setTag("");
   };

   const addTag = (tag: string) => {
      if (tag === "") return;
      setFieldValue("Tags", [...Tags!, tag]);
      setTag("");
   };

   return (
      <MultiSelect
         w={"50%"}
         label={<Text ta={"start"}>Select tags</Text>}
         placeholder={"Add a tag"}
         variant={"default"}
         value={Tags}
         onKeyDown={handleKeyDown}
         rightSection={!!tag.length ? <AddIcon cursor={"pointer"} onClick={e => addTag(tag)} fontSize={14} /> : <></>}
         getCreateLabel={query => ` +  ${query}`}
         searchable
         creatable
         searchValue={tag}
         onSearchChange={setTag}
         onChange={tags => setFieldValue("Tags", tags)}
         onCreate={query => {
            addTag(query);
            return query;
         }}
         data={Tags!}
      />
   );
};

export default TagsInput;