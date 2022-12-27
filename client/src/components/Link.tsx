import React, { FC, PropsWithChildren } from "react";
import { Link as ReactLink } from "react-router-dom";

interface IProps extends PropsWithChildren {
   to: string;
}

const Link: FC<IProps> = ({ children, to }) => {
   return (
      <ReactLink style={{ textDecoration: "none", color: "inherit" }} to={to}>
         {children}
      </ReactLink>
   );
};

export default Link;