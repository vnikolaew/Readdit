import { Button, useMantineTheme } from "@mantine/core";
import React, { FC, PropsWithChildren } from "react";
import { IconType } from "react-icons";

interface IProps extends PropsWithChildren {
   isActive: boolean;
   Icon: IconType;
   onClick?: React.MouseEventHandler<HTMLButtonElement>;
}

const FeedType: FC<IProps> = ({ isActive, Icon, children, onClick }) => {
   const theme = useMantineTheme();
   return (
      <Button onClick={onClick}
              radius={"lg"}
              variant={"default"}
              sx={theme => ({
                 border: "none",
                 transitionDuration: "100ms",
                 color: isActive ? theme.colors.gray[0] : theme.colors.gray[6],
                 backgroundColor: isActive ? theme.colors.gray[8] : "transparent",
                 "&:hover": {
                    backgroundColor: theme.colors.gray[7],
                 },
              })}
              leftIcon={<Icon size={16} color={isActive ? theme.colors.gray[0] : theme.colors.gray[6]} />}>
         {children}
      </Button>
   );
};

export default FeedType;