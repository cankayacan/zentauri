import { h, render } from "preact";
import { parseColor as c } from "onejs/utils/color-parser";
import { useEffect } from "preact/hooks";

const App = () => {
  const lobbyPlayer = require("lobbyPlayer");
  const renderTexture = lobbyPlayer.renderTexture;
  const animator = lobbyPlayer.animator;

  log("render app");

  useEffect(() => {
    setInterval(() => {
      animator.SetTrigger("Wave");
    }, 10000);
  }, []);

  return (
    <div
      style={{
        backgroundImage: "images/bg.png",
        backgroundColor: "#15202C",
        width: "100%",
        height: "100%",
      }}
      class="p-10"
    >
      <label text="Main" />
      <image image={renderTexture} />
      <div>
        <button
          name="ok"
          text="Start"
          style={{
            backgroundColor: "#4E7CFF",
            color: "#FFFFFF",
            width: "200px",
            height: "40px",
            fontSize: "16px",
            borderRadius: 5,
          }}
        />
      </div>
    </div>
  );
};

render(<App />, document.body);
