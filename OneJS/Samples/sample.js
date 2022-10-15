Object.defineProperty(exports, "__esModule", { value: true });
const color_palettes_1 = require("onejs/utils/color-palettes");
const color_parser_1 = require("onejs/utils/color-parser");
const preact_1 = require("preact");
const compact_1 = require("preact/compact");
const hooks_1 = require("preact/hooks");
const tween_1 = require("tweenjs/tween");
const UnityEngine_1 = require("UnityEngine");
let plane = UnityEngine_1.GameObject.CreatePrimitive(UnityEngine_1.PrimitiveType.Plane);
plane.GetComp(UnityEngine_1.MeshRenderer).material.color = (0, color_parser_1.namedColor)("maroon");
plane.transform.localScale = new UnityEngine_1.Vector3(10, 1, 10);
var cam = UnityEngine_1.GameObject.Find("Main Camera");
cam.transform.position = new UnityEngine_1.Vector3(0, 30, -60);
cam.transform.LookAt(new UnityEngine_1.Vector3(0, 10, 0));
UnityEngine_1.Physics.gravity = new UnityEngine_1.Vector3(0, -30, 0);
let balls = [];
spawnBalls();
function spawnBalls() {
    for (let i = 0; i < balls.length; i++) {
        UnityEngine_1.Object.Destroy(balls[i]);
    }
    balls = [];
    for (let i = 0; i < 50; i++) {
        createRandomBall();
    }
    setTimeout(spawnBalls, 15000);
}
function createRandomBall() {
    let ball = UnityEngine_1.GameObject.CreatePrimitive(UnityEngine_1.PrimitiveType.Sphere);
    ball.GetComp(UnityEngine_1.MeshRenderer).material.color = (0, color_parser_1.parseColor)(color_palettes_1.palettes[UnityEngine_1.Random.Range(0, 99)][2]);
    ball.transform.position = UnityEngine_1.Random.insideUnitSphere * 10 + new UnityEngine_1.Vector3(0, 30, 0);
    let rb = ball.AddComp(UnityEngine_1.Rigidbody);
    rb.collisionDetectionMode = UnityEngine_1.CollisionDetectionMode.Continuous;
    rb.drag = 0.3;
    let pm = new UnityEngine_1.PhysicMaterial();
    pm.bounciness = 1;
    ball.GetComp(UnityEngine_1.SphereCollider).material = pm;
    balls.push(ball);
}
const Dot = (0, compact_1.forwardRef)((props, ref) => {
    var _a, _b;
    const color = (_a = props.color) !== null && _a !== void 0 ? _a : (0, color_parser_1.namedColor)("tomato");
    const size = (_b = props.size) !== null && _b !== void 0 ? _b : 80;
    const defaultOuterStyle = {
        width: size, height: size, backgroundColor: "white", borderRadius: size / 2, position: "Absolute", justifyContent: "Center", alignItems: "Center", left: -size / 2, top: -size / 2
    };
    const defaultInnerStyle = {
        width: size - 4, height: size - 4, backgroundColor: color, borderRadius: (size - 4) / 2,
        backgroundImage: props.image, unityBackgroundScaleMode: "ScaleAndCrop",
        justifyContent: "Center", alignItems: "Center", color: "white"
    };
    return ((0, preact_1.h)("div", { ref: ref, onPointerDown: props.onPointerDown, style: Object.assign(Object.assign({}, props.style), defaultOuterStyle) },
        (0, preact_1.h)("div", { style: defaultInnerStyle }, props.children)));
});
const App = () => {
    const ref = (0, hooks_1.useRef)();
    const dot1ref = (0, hooks_1.useRef)();
    const dot2ref = (0, hooks_1.useRef)();
    const [pos1, setPos1] = (0, hooks_1.useState)({ x: 0, y: 0 });
    const [pos2, setPos2] = (0, hooks_1.useState)({ x: 0, y: 0 });
    const [inited, setInited] = (0, hooks_1.useState)(false);
    let pointerDowned = false;
    let offsetPosition = { x: 0, y: 0 };
    (0, hooks_1.useEffect)(() => {
        let width = ref.current.ve.resolvedStyle.width;
        let height = ref.current.ve.resolvedStyle.height;
        let p1 = { x: width / 6 * 2, y: height / 6 * 2 };
        let p2 = { x: width / 6 * 4, y: height / 6 * 4 };
        setInited(true);
        setPos1(p1);
        setPos2(p2);
        const tween = new tween_1.Tween(p2).to({ x: p2.x, y: p2.y - height / 6 * 2 }, 5000)
            .easing(tween_1.Easing.Quadratic.InOut).onUpdate(() => {
            dot2ref.current.style.translate = p2;
            ref.current.ve.MarkDirtyRepaint();
        }).repeat(Infinity).yoyo(true).start();
    }, []);
    (0, hooks_1.useEffect)(() => {
        ref.current.ve.generateVisualContent = onGenerateVisualContent;
        ref.current.ve.MarkDirtyRepaint();
    }, [inited, pos2]);
    function onGenerateVisualContent(mgc) {
        var paint2D = mgc.painter2D;
        let { x: x1, y: y1 } = pos1;
        let { x: x2, y: y2 } = pos2;
        paint2D.strokeColor = UnityEngine_1.Color.white;
        paint2D.lineWidth = 10;
        paint2D.BeginPath();
        paint2D.MoveTo(new UnityEngine_1.Vector2(x1, y1));
        paint2D.BezierCurveTo(new UnityEngine_1.Vector2(x1 + 180, y1), new UnityEngine_1.Vector2(x2 - 180, y2), new UnityEngine_1.Vector2(x2, y2));
        paint2D.Stroke();
    }
    function onPointerDown(evt) {
        pointerDowned = true;
        offsetPosition = { x: evt.position.x - pos1.x, y: evt.position.y - pos1.y };
    }
    function onPointerUp(evt) {
        pointerDowned = false;
    }
    function onPointerLeave(evt) {
        pointerDowned = false;
    }
    function onPointerMove(evt) {
        if (!pointerDowned)
            return;
        pos1.x = evt.position.x - offsetPosition.x;
        pos1.y = evt.position.y - offsetPosition.y;
        dot1ref.current.style.translate = pos1;
        ref.current.ve.MarkDirtyRepaint();
    }
    return ((0, preact_1.h)("div", { ref: ref, onPointerUp: onPointerUp, onPointerLeave: onPointerLeave, onPointerMove: onPointerMove, style: { width: "100%", height: "100%" } },
        (0, preact_1.h)(Dot, { ref: dot1ref, onPointerDown: onPointerDown, style: { translate: pos1, display: inited ? "Flex" : "None" } }, "Drag Me"),
        (0, preact_1.h)(Dot, { ref: dot2ref, image: __dirname + "/controller.png", style: { translate: pos2, display: inited ? "Flex" : "None" } })));
};
(0, preact_1.render)((0, preact_1.h)(App, null), document.body);
function animate(time) {
    requestAnimationFrame(animate);
    (0, tween_1.update)(time);
}
requestAnimationFrame(animate);
