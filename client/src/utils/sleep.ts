export const sleep = async (ms: number): Promise<void> => {
    return await new Promise((res) =>
        setTimeout(() => {
            res()
        }, ms)
    );
};
