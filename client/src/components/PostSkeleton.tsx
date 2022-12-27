import React, { FC } from "react";
import { BoxProps, Group, Skeleton } from "@mantine/core";

const PostSkeleton: FC<BoxProps> = (props) => {
   return (
      <Group w={"100%"} mb={3} p="6" bg={"transparent"} {...props}>
         <Skeleton width={"100%"} height={50} circle />
         <Skeleton width={"100%"} height={8} radius="xl" />
         <Skeleton width={"100%"} height={8} mt={6} radius="xl" />
         <Skeleton width={"100%"} height={8} mt={6} radius="xl" />
         <Skeleton height={8} mt={6} width="70%" radius="xl" />
      </Group>
   );
};

export default PostSkeleton;