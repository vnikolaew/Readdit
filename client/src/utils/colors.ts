import { ColorProps } from "@chakra-ui/react";

export function getColorByScore(score: number): ColorProps["color"] {
   if (score < -10) return "red.900";
   if (score < -8) return "red.800";
   if (score < -6) return "red.700";
   if (score < -4) return "red.600";

   if (score < -2) return "orange.900";
   if (score < 0) return "orange.800";
   if (score < 2) return "orange.700";
   if (score < 4) return "orange.600";

   if (score < 6) return "yellow.900";
   if (score < 10) return "yellow.800";
   if (score < 20) return "yellow.700";
   if (score < 40) return "yellow.600";

   if (score < 50) return "green.900";
   if (score < 100) return "green.800";
   if (score < 200) return "green.700";
   if (score < 400) return "green.600";
}