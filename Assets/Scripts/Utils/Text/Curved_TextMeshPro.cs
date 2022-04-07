using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Utils.Text
{
    public class Curved_TextMeshPro : MonoBehaviour
    {
        public AnimationCurve VertexCurve;
        public float CurveScale = 100.0f;
        public TMP_Text Text ;

        private Curved_TextMeshProEditor _editor;

        void Awake()
        {
            SetTextComponent();
            ChangeTextVertices();
            _editor = Curved_TextMeshProEditor.Create(this);
        }

#region EDITOR_API

        private void OnEnable() => _editor?.OnEnable();

        private void OnDisable() => _editor?.OnDisable();

        
       [Button(ButtonSizes.Large), HorizontalGroup("Btns")]
        private void SetCurveTemplate()
        {
            Curved_TextMeshProEditor.ApplyCurve(this);
            SetTextComponent();
            ChangeTextVertices();
        }

        [Button(ButtonSizes.Large), HorizontalGroup("Btns")]
        private void ApplyChanges()
        {
            SetTextComponent();
            ChangeTextVertices();
        }

        private void SetTextComponent() => Text = Text ? Text : gameObject.GetComponent<TMP_Text>();

        // [Button(ButtonSizes.Large), HorizontalGroup("Btns")]
        // private void KeepScale() => Curved_TextMeshProEditor.KeepScale(this);
        //
        // [Button(ButtonSizes.Large), HorizontalGroup("Btns")]
        // private void ApplyScale() => Curved_TextMeshProEditor.ApplyScale(this);
        
#endregion

        public void ChangeTextVertices()
        {
            VertexCurve.preWrapMode = WrapMode.Clamp;
            VertexCurve.postWrapMode = WrapMode.Clamp;
            
            Vector3[] vertices;
            Matrix4x4 matrix;

            Text.ForceMeshUpdate(); // Generate the mesh and populate the textInfo with data we can use and manipulate.

            var textInfo = Text.textInfo;
            var characterCount = textInfo.characterCount;


            if (characterCount == 0)
                return;

            var boundsMinX = Text.bounds.min.x;
            var boundsMaxX = Text.bounds.max.x;

            for (var i = 0; i < characterCount; i++)
            {
                var characterInfo = textInfo.characterInfo[i];
                if (!characterInfo.isVisible)
                    continue;

                var vertexIndex = characterInfo.vertexIndex;

                // Get the index of the mesh used by this character.
                var materialIndex = characterInfo.materialReferenceIndex;

                vertices = textInfo.meshInfo[materialIndex].vertices;

                // Compute the baseline mid point for each character
                Vector3 offsetToMidBaseline = new Vector2((vertices[vertexIndex + 0].x + vertices[vertexIndex + 2].x) / 2,
                    characterInfo.baseLine);
                //float offsetY = VertexCurve.Evaluate((float)i / characterCount + loopCount / 50f); // Random.Range(-0.25f, 0.25f);

                // Apply offset to adjust our pivot point.
                vertices[vertexIndex + 0] += -offsetToMidBaseline;
                vertices[vertexIndex + 1] += -offsetToMidBaseline;
                vertices[vertexIndex + 2] += -offsetToMidBaseline;
                vertices[vertexIndex + 3] += -offsetToMidBaseline;

                // Compute the angle of rotation for each character based on the animation curve
                matrix = CalcMatrix(offsetToMidBaseline, boundsMinX, boundsMaxX);

                vertices[vertexIndex + 0] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 0]);
                vertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 1]);
                vertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 2]);
                vertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 3]);

                vertices[vertexIndex + 0] += offsetToMidBaseline;
                vertices[vertexIndex + 1] += offsetToMidBaseline;
                vertices[vertexIndex + 2] += offsetToMidBaseline;
                vertices[vertexIndex + 3] += offsetToMidBaseline;
            }


            // Upload the mesh with the revised information
            Text.UpdateVertexData();
        }

        private Matrix4x4 CalcMatrix(Vector3 offsetToMidBaseline, float boundsMinX, float boundsMaxX)
        {
            Matrix4x4 matrix;
            var x0 = (offsetToMidBaseline.x - boundsMinX) /
                     (boundsMaxX - boundsMinX); // Character's position relative to the bounds of the mesh.
            var x1 = x0 + 0.0001f;
            var y0 = VertexCurve.Evaluate(x0) * CurveScale;
            var y1 = VertexCurve.Evaluate(x1) * CurveScale;

            var horizontal = new Vector3(1, 0, 0);
            //Vector3 normal = new Vector3(-(y1 - y0), (x1 * (boundsMaxX - boundsMinX) + boundsMinX) - offsetToMidBaseline.x, 0);
            var tangent = new Vector3(x1 * (boundsMaxX - boundsMinX) + boundsMinX, y1) -
                          new Vector3(offsetToMidBaseline.x, y0);

            const float RAD_TO_DEGREES = 57.2957795f;
            var dot = Vector3.Dot(horizontal, tangent.normalized);
            var angle = Mathf.Acos(dot) * RAD_TO_DEGREES;
            var cross = Vector3.Cross(horizontal, tangent);
            var finalAngle = cross.z > 0 ? angle : 360 - angle;

            matrix = Matrix4x4.TRS(new Vector3(0, y0, 0), Quaternion.Euler(0, 0, finalAngle), Vector3.one);
            return matrix;
        }
    }
}
