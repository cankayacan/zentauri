import { h, render } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Button } from "./Button";
import { Spinner } from "./Spinner";

const App = () => {
  const [loading, setLoading] = useState(false);
  const lobby = require("lobby");
  const playerRenderTexture = lobby.playerRenderTexture;
  const playerAnimator = lobby.playerAnimator;

  const wave = () => playerAnimator.SetTrigger("Wave");

  useEffect(() => {
    setInterval(() => wave(), 10000);
    wave();
  }, []);

  const startGame = () => {
    setLoading(true);
    lobby.StartGame();
  };

  return (
    <div
      class="flex-row w-max h-max"
      style={{ backgroundImage: "images/bg.png" }}
    >
      <div class="relative w-28">
        {loading && (
          <div class="absolute flex-row items-center bottom-5 left-5">
            <div style={{ width: 25, marginRight: 5 }}>
              <Spinner radius={25} thickness={2.5} />
            </div>
            <div style={{ color: "#FFFFFF", fontSize: "8px" }}>Loading...</div>
          </div>
        )}
      </div>

      <div class="grow">
        <image image={playerRenderTexture} />
      </div>

      <div class="self-end w-28 pb-5 pr-5 ">
        <Button text="Start" onClick={startGame} enabled={!loading} />
      </div>
    </div>
  );
};

render(<App />, document.body);
