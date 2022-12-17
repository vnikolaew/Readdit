export const __ENV__ = process.env.REACT_APP_NODE_ENV!;

export const __DEV__ = __ENV__ === "development";
export const __PROD__ = __ENV__ === "production";
