import {
  MantineProvider,
  AppShell,
  Header,
  ColorSchemeProvider,
  ColorScheme,
  Text,
} from "@mantine/core";
import { createBrowserRouter, RouterProvider, Outlet } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import MainHeader, { HeaderLink } from "./Features/Components/MainHeader";
import Home from "./Features/Home";
import { useState } from "react";

const router = createBrowserRouter([
  {
    element: <AppShellWrapper />,
    children: [
      {
        path: "/",
        element: <Home />,
      },
    ],
  },
]);

const Links: HeaderLink[] = [{ link: "/", label: "Home" }];

const queryClient = new QueryClient();

function App() {
  const [colorScheme, setColorScheme] = useState<ColorScheme>("light");
  const toggleColorScheme = (value?: ColorScheme) =>
    setColorScheme(value || (colorScheme === "dark" ? "light" : "dark"));

  return (
    <QueryClientProvider client={queryClient}>
      <ColorSchemeProvider
        colorScheme={colorScheme}
        toggleColorScheme={toggleColorScheme}
      >
        <MantineProvider
          theme={{ colorScheme }}
          withGlobalStyles
          withNormalizeCSS
        >
          <RouterProvider router={router} />
        </MantineProvider>
      </ColorSchemeProvider>
    </QueryClientProvider>
  );
}

function AppShellWrapper() {
  return (
    <AppShell
      padding="md"
      header={
        <Header height={80} mb={120} p="md">
          <MainHeader links={Links} />
        </Header>
      }
      styles={(theme) => ({
        main: {
          backgroundColor:
            theme.colorScheme === "dark"
              ? theme.colors.dark[5]
              : theme.colors.gray[0],
        },
      })}
    >
      <Outlet />
    </AppShell>
  );
}
export default App;
