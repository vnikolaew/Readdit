import * as yup from "yup";

export const validationSchema = yup.object({
   CommunityId: yup.string().required(),
   Content: yup.string().min(6).max(200).required(),
   Title: yup.string().min(6).max(200).required(),
   Media: yup.object().test("Media", "Please upload a valid media",
      x => x === null || x instanceof File),
   Tags: yup.array().of(yup.string().required()),
});