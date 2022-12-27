import { MantineTheme } from "@mantine/core";

export function getColorByScore(score: number, theme: MantineTheme): string {
   if (score < -10) return theme.colors.red[9];
   if (score < -8) return theme.colors.red[8];
   if (score < -6) return theme.colors.red[7];
   if (score < -4) return theme.colors.red[6];

   if (score < -2) return theme.colors.orange[9];
   if (score < 0) return theme.colors.orange[8];
   if (score < 2) return theme.colors.orange[7];
   if (score < 4) return theme.colors.orange[6];

   if (score < 6) return theme.colors.yellow[9];
   if (score < 10) return theme.colors.yellow[8];
   if (score < 20) return theme.colors.yellow[7];
   if (score < 40) return theme.colors.yellow[6];

   if (score < 50) return theme.colors.green[9];
   if (score < 100) return theme.colors.green[8];
   if (score < 200) return theme.colors.green[7];

   return theme.colors.green[6];
}